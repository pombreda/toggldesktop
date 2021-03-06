//
//  PreferencesWindowController.h
//
//  Copyright (c) 2014 TogglDesktop developers. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "MASShortcutView+UserDefaults.h"
#import "DisplayCommand.h"
#import "AutocompleteItem.h"
#import "AutocompleteDataSource.h"
#import "NSCustomComboBox.h"

extern NSString *const kPreferenceGlobalShortcutShowHide;
extern NSString *const kPreferenceGlobalShortcutStartStop;

@interface PreferencesWindowController : NSWindowController <NSTextFieldDelegate, NSTableViewDataSource>
@property IBOutlet NSTextField *hostTextField;
@property IBOutlet NSTextField *portTextField;
@property IBOutlet NSTextField *usernameTextField;
@property IBOutlet NSTextField *passwordTextField;
@property IBOutlet NSButton *useProxyButton;
@property IBOutlet NSButton *useIdleDetectionButton;
@property IBOutlet NSButton *recordTimelineCheckbox;
@property IBOutlet NSButton *menubarTimerCheckbox;
@property IBOutlet NSButton *menubarProjectCheckbox;
@property IBOutlet NSButton *dockIconCheckbox;
@property IBOutlet NSButton *ontopCheckbox;
@property IBOutlet NSButton *reminderCheckbox;
@property IBOutlet MASShortcutView *showHideShortcutView;
@property IBOutlet MASShortcutView *startStopShortcutView;
@property IBOutlet NSTextField *idleMinutesTextField;
@property IBOutlet NSButton *focusOnShortcutCheckbox;
@property IBOutlet NSTextField *reminderMinutesTextField;
@property IBOutlet NSButton *autodetectProxyCheckbox;
@property IBOutlet NSTextField *autotrackerTerm;
@property IBOutlet NSCustomComboBox *autotrackerProject;
@property IBOutlet NSTableView *autotrackerRulesTableView;
@property IBOutlet NSButton *remindMon;
@property IBOutlet NSButton *remindTue;
@property IBOutlet NSButton *remindWed;
@property IBOutlet NSButton *remindThu;
@property IBOutlet NSButton *remindFri;
@property IBOutlet NSButton *remindSat;
@property IBOutlet NSButton *remindSun;
@property IBOutlet NSTextField *remindStarts;
@property IBOutlet NSTextField *remindEnds;
- (IBAction)idleMinutesChange:(id)sender;
- (IBAction)useProxyButtonChanged:(id)sender;
- (IBAction)hostTextFieldChanged:(id)sender;
- (IBAction)portTextFieldChanged:(id)sender;
- (IBAction)usernameTextFieldChanged:(id)sender;
- (IBAction)passwordTextFieldChanged:(id)sender;
- (IBAction)useIdleDetectionButtonChanged:(id)sender;
- (IBAction)recordTimelineCheckboxChanged:(id)sender;
- (IBAction)menubarTimerCheckboxChanged:(id)sender;
- (IBAction)menubarProjectCheckboxChanged:(id)sender;
- (IBAction)dockIconCheckboxChanged:(id)sender;
- (IBAction)ontopCheckboxChanged:(id)sender;
- (IBAction)reminderCheckboxChanged:(id)sender;
- (IBAction)focusOnShortcutCheckboxChanged:(id)sender;
- (IBAction)reminderMinutesChanged:(id)sender;
- (IBAction)autodetectProxyCheckboxChanged:(id)sender;
- (IBAction)addAutotrackerRule:(id)sender;
- (IBAction)remindWeekChanged:(id)sender;
- (IBAction)remindTimesChanged:(id)sender;

@property DisplayCommand *originalCmd;
@property uint64_t user_id;
@end
