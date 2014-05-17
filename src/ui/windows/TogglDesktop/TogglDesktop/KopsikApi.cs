﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;

namespace TogglDesktop
{
    static class KopsikApi
    {
        public static IntPtr ctx = IntPtr.Zero;

        private const string dll = "TogglDesktopDLL.dll";
        private const CharSet charset = CharSet.Ansi;
        private const CallingConvention convention = CallingConvention.Cdecl;

        // Models

        [StructLayout(LayoutKind.Sequential, CharSet = charset)]
        public struct KopsikTimeEntryViewItem
        {
            public Int64 DurationInSeconds;
            public string Description;
            public string ProjectAndTaskLabel;
            public UInt64 WID;
            public UInt64 PID;
            public UInt64 TID;
            public string Duration;
            public string Color;
            public string GUID;
            [MarshalAs(UnmanagedType.Bool)]
            public bool Billable;
            public string Tags;
            public UInt64 Started;
            public UInt64 Ended;
            public string StartTimeString;
            public string EndTimeString;
            public UInt64 UpdatedAt;
            public string DateHeader;
            public string DateDuration;
            [MarshalAs(UnmanagedType.Bool)]
            public bool DurOnly;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsHeader;
            public IntPtr Next;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = charset)]
        public struct KopsikAutocompleteItem
        {
            public string Text;
            public string Description;
            public string ProjectAndTaskLabel;
            public UInt64 TaskID;
            public UInt64 ProjectID;
            public IntPtr Next;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = charset)]
        public struct KopsikViewItem
        {
            public UInt64 ID;
            public string GUID;
            public string Name;
            public IntPtr Next;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = charset)]
        public struct KopsikSettingsViewItem
        {
            public bool UseProxy;
            public string ProxyHost;
            public UInt64 ProxyPort;
            public string ProxyUsername;
            public string ProxyPassword;
            public bool UseIdleDetection;
            public bool MenubarTimer;
            public bool DockIcon;
            public bool OnTop;
            public bool Reminder;
            public bool RecordTimeline;
        }

        // Callbacks

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayError(
            string errmsg,
            bool user_error);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayUpdate(
            bool is_update,
            string url,
            string version);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayOnlineState(
            bool is_online);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayURL(
            string url);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayLogin(
            bool open,
            UInt64 user_id);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayReminder(
            string title,
            string informative_text);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayTimeEntryList(
            bool open,
            ref KopsikTimeEntryViewItem first);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayAutocomplete(
            ref KopsikAutocompleteItem first);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayViewItems(
            ref KopsikViewItem first);
        
        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayTimeEntryEditor(
            bool open,
            ref KopsikTimeEntryViewItem te,
            string focused_field_name);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplaySettings(
            bool open,
            ref KopsikSettingsViewItem settings);

        [UnmanagedFunctionPointer(convention)]
        public delegate void KopsikDisplayTimerState(
            ref KopsikTimeEntryViewItem te);

        // Initialize/destroy an instance of the app

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern System.IntPtr kopsik_context_init(
            string app_name,
            string app_version);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_context_clear(
            IntPtr context);

        // DB path must be configured from UI

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern System.IntPtr kopsik_set_db_path(
            IntPtr context,
            string path);

        // Log path must be configured from UI

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern System.IntPtr kopsik_set_log_path(
            string path);

        // Log level is optional

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern System.IntPtr kopsik_set_log_level(
            string level);

        // API URL can be overriden from UI. Optional

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern System.IntPtr kopsik_set_api_url(
            IntPtr context,
            string path);

        // WebSocket URL can be overriden from UI. Optional

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern System.IntPtr kopsik_set_websocket_url(
            string path);

        // Configure the UI callbacks. Required.

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        private static extern void kopsik_on_error(
            IntPtr context,
            KopsikDisplayError cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_update(
            IntPtr context,
            KopsikDisplayUpdate cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_online_state(
            IntPtr context,
            KopsikDisplayOnlineState cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_url(
            IntPtr context,
            KopsikDisplayURL cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_login(
            IntPtr context,
            KopsikDisplayLogin cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_reminder(
            IntPtr context,
            KopsikDisplayReminder cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_time_entry_list(
            IntPtr context,
            KopsikDisplayTimeEntryList cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_autocomplete(
            IntPtr context,
            KopsikDisplayAutocomplete cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_workspace_select(
            IntPtr context,
            KopsikDisplayViewItems cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_client_select(
            IntPtr context,
            KopsikDisplayViewItems cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_tags(
            IntPtr context,
            KopsikDisplayViewItems cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_time_entry_editor(
            IntPtr context,
            KopsikDisplayTimeEntryEditor cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_settings(
            IntPtr context,
            KopsikDisplaySettings cb);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_on_timer_state(
            IntPtr context,
            KopsikDisplayTimerState cb);

        // After UI callbacks are configured, start pumping UI events

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        private static extern bool kopsik_context_start_events(
            IntPtr context);

        // User interaction with the app

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_login(
            IntPtr context,
            string email,
            string password);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_password_forgot(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_open_in_browser(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_get_support(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_feedback_send(
            IntPtr context,
            string topic,
            string details,
            string filename);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_view_time_entry_list(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_edit(
            IntPtr context,
            string guid,
            bool edit_running_time_entry,
            string focused_field_name);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_edit_preferences(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_continue(
            IntPtr context,
            string guid);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_continue_latest(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_delete_time_entry(
            IntPtr context,
            string guid);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_time_entry_duration(
            IntPtr context,
            string guid,
            string value);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_time_entry_project(
            IntPtr context,
            string guid,
            UInt64 task_id,
            UInt64 project_id,
            string project_guid);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_time_entry_start_iso_8601(
            IntPtr context,
            string guid,
            string value);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_time_entry_end_iso_8601(
            IntPtr context,
            string guid,
            string value);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_time_entry_tags(
            IntPtr context,
            string guid,
            string value);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_time_entry_billable(
            IntPtr context,
            string guid,
            bool billable);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_time_entry_description(
            IntPtr context,
            string guid,
            string value);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_stop(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_stop_running_time_entry_at(
            IntPtr context,
            UInt64 at);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_settings(
            IntPtr context,
            bool use_idle_detection,
            bool menubar_timer,
            bool dock_icon,
            bool on_top,
            bool reminder);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_proxy_settings(
            IntPtr context,
            bool use_proxy,
            string proxy_host,
            UInt64 proxy_port,
            string proxy_username,
            string proxy_password);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_logout(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_clear_cache(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_start(
            IntPtr context,
            string description,
            string duration,
            UInt64 task_id,
            UInt64 project_id);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_add_project(
            IntPtr context,
            string time_entry_guid,
            UInt64 workspace_id,
            UInt64 client_id,
            string project_name,
            bool is_private);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_check_for_updates(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_set_update_channel(
            IntPtr context,
            string update_channel);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_sync(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_timeline_toggle_recording(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_set_sleep(
            IntPtr context);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_set_wake(
            IntPtr context);

        // Shared helpers

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_parse_time(
            string input,
            ref int hours,
            ref int minutes);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_format_duration_in_seconds_hhmmss(
            IntPtr duration_in_seconds,
            ref string str,
            int max_strlen);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_format_duration_in_seconds_hhmm(
            Int64 duration_in_seconds,
            ref string str,
            int max_strlen);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern bool kopsik_format_duration_in_seconds_pretty_hhmm(
            Int64 duration_in_seconds,
            ref string str,
            int max_strlen);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern Int64 kopsik_parse_duration_string_into_seconds(
            string duration_string);

        [DllImport(dll, CharSet = charset, CallingConvention = convention)]
        public static extern void kopsik_debug(
            IntPtr context,
            string text);

        // Events for C#

        public static event KopsikApi.KopsikDisplayError OnError = delegate { };
        public static event KopsikApi.KopsikDisplayUpdate OnUpdate = delegate { };
        public static event KopsikApi.KopsikDisplayOnlineState OnOnlineState = delegate { };
        public static event KopsikApi.KopsikDisplayLogin OnLogin = delegate { };
        public static event KopsikApi.KopsikDisplayReminder OnReminder = delegate { };
        public static event KopsikApi.KopsikDisplayTimeEntryList OnTimeEntryList = delegate { };
        public static event KopsikApi.KopsikDisplayAutocomplete OnAutocomplete = delegate { };
        public static event KopsikApi.KopsikDisplayTimeEntryEditor OnTimeEntryEditor = delegate { };
        public static event KopsikApi.KopsikDisplayViewItems OnWorkspaceSelect = delegate { };
        public static event KopsikApi.KopsikDisplayViewItems OnClientSelect = delegate { };
        public static event KopsikApi.KopsikDisplayViewItems OnTags = delegate { };
        public static event KopsikApi.KopsikDisplaySettings OnSettings = delegate { };
        public static event KopsikApi.KopsikDisplayTimerState OnTimerState = delegate { };

        // Start

        public static bool Start()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            ctx = kopsik_context_init("windows_native_app", versionInfo.ProductVersion);

            StringBuilder sb = new StringBuilder();
            sb.Append("sizeof(KopsikTimeEntryViewItem)=");
            sb.Append(Marshal.SizeOf(new KopsikTimeEntryViewItem()));
            sb.Append(", sizeof(KopsikAutocompleteItem)=");
            sb.Append(Marshal.SizeOf(new KopsikAutocompleteItem()));
            sb.Append("sizeof(KopsikViewItem)=");
            sb.Append(Marshal.SizeOf(new KopsikViewItem()));
            sb.Append(", sizeof(KopsikSettingsViewItem)=");
            sb.Append(Marshal.SizeOf(new KopsikSettingsViewItem()));
            kopsik_debug(ctx, sb.ToString());

            // Wire up events
            kopsik_on_error(ctx, OnError);
            kopsik_on_update(ctx, OnUpdate);                
            kopsik_on_online_state(ctx, OnOnlineState);
            kopsik_on_login(ctx, OnLogin);
            kopsik_on_reminder(ctx, OnReminder);
            kopsik_on_time_entry_list(ctx, OnTimeEntryList);
            kopsik_on_autocomplete(ctx, OnAutocomplete);
            kopsik_on_time_entry_editor(ctx, OnTimeEntryEditor);
            kopsik_on_workspace_select(ctx, OnWorkspaceSelect);
            kopsik_on_client_select(ctx, OnClientSelect);
            kopsik_on_tags(ctx, OnTags);
            kopsik_on_settings(ctx, OnSettings);
            kopsik_on_timer_state(ctx, OnTimerState);
            kopsik_on_url(ctx, OnURL);

            // Configure log, db path
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "Kopsik");
            System.IO.Directory.CreateDirectory(path);
            kopsik_set_log_path(Path.Combine(path, "kopsik.log"));
            kopsik_set_log_level("debug");
            kopsik_set_db_path(ctx, Path.Combine(path, "kopsik.db"));

            // Start pumping UI events
            return kopsik_context_start_events(ctx);
        }

        private static void OnURL(string url)
        {
            Process.Start(url);
        }
    }
}
