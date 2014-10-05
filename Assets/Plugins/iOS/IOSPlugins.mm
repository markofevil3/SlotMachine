//=============================================================================
//	KeyChainDLL.mm
//
//	iOS functionality for the Unity KeyChain store data plug-in.
//=============================================================================

#import "PDKeychainBindings.h"
#include "OpenUDID.h"

#include <iostream>
using namespace std;

extern "C" {
	void IOSSetStringForKey(const char* value, const char* keyName) {
		[[PDKeychainBindings sharedKeychainBindings] setObject:[[NSString stringWithUTF8String:value] retain] forKey:[[NSString stringWithUTF8String:keyName] retain]];
	}
	
	char* IOSGetStringForKey(const char* keyName) {
		NSString* resultStr = [[PDKeychainBindings sharedKeychainBindings] stringForKey:[[NSString stringWithUTF8String:keyName] retain]];
		const char *cStr = (resultStr ? [resultStr UTF8String] : "undefined");
    int count = strlen(cStr);
    char* result = (char *)malloc(count + 1);
    strcpy(result, cStr);
    
    return result;
	}
	
	void IOSRemoveObjectForKey(const char* keyName) {
		[[PDKeychainBindings sharedKeychainBindings] removeObjectForKey:[[NSString stringWithUTF8String:keyName] retain]];
	}
  
  char* GetOpenUDID() {
    NSString* resultStr = [OpenUDID value];
    const char *cStr = (resultStr ? [resultStr UTF8String] : "undefined");
    int count = strlen(cStr);
    char* result = (char *)malloc(count + 1);
    strcpy(result, cStr);
    
    return result;
  }
}