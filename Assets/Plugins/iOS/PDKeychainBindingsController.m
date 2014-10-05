//
//  PDKeychainBindingsController.m
//  PDKeychainBindingsController
//
//  Created by Carl Brown on 7/10/11.
//  Copyright 2011 PDAgent, LLC. Released under MIT License.
//

//  There's (understandably) a lot of controversy about how (and whether)
//   to use the Singleton pattern for Cocoa.  I am here because I'm
//   trying to emulate existing Singleton (NSUserDefaults) behavior
//
//   and I'm using the singleton methodology from
//   http://www.duckrowing.com/2010/05/21/using-the-singleton-pattern-in-objective-c/
//   because it seemed reasonable

// define some LLVM3 macros if the code is compiled with a different compiler (ie LLVMGCC42)
#ifndef __has_feature
#define __has_feature(x) 0
#endif
#ifndef __has_extension
#define __has_extension __has_feature // Compatibility with pre-3.0 compilers.
#endif

#if __has_feature(objc_arc) && __clang_major__ >= 3
#define II_ARC_ENABLED 1
#endif // __has_feature(objc_arc)

#if II_ARC_ENABLED
#define II_RETAIN(xx)  ((void)(0))
#define II_RELEASE(xx)  ((void)(0))
#define II_AUTORELEASE(xx)  (xx)
#define IIBRIDGE    IIBRIDGE
#else
#define II_RETAIN(xx)           [xx retain]
#define II_RELEASE(xx)          [xx release]
#define II_AUTORELEASE(xx)      [xx autorelease]
#define IIBRIDGE
#endif


#import "PDKeychainBindingsController.h"
#import <Security/Security.h>

static PDKeychainBindingsController *sharedInstance = nil;

@implementation PDKeychainBindingsController

#pragma mark -
#pragma mark Keychain Access

- (NSString*)serviceName {
	return [[NSBundle mainBundle] bundleIdentifier];
}

- (NSString*)stringForKey:(NSString*)key {
	OSStatus status;
#if TARGET_OS_IPHONE
  NSDictionary *query = [NSDictionary dictionaryWithObjectsAndKeys:(id)kCFBooleanTrue, kSecReturnData,
                         kSecClassGenericPassword, kSecClass,
                         key, kSecAttrAccount,
                         [self serviceName], kSecAttrService,
                         nil];
	
  CFDataRef stringData = NULL;
  status = SecItemCopyMatching((IIBRIDGE CFDictionaryRef)query, (CFTypeRef*)&stringData);
#else //OSX
  //SecKeychainItemRef item = NULL;
  UInt32 stringLength;
  void *stringBuffer;
  status = SecKeychainFindGenericPassword(NULL, (uint) [[self serviceName] lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [[self serviceName] UTF8String],
                                          (uint) [key lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [key UTF8String],
                                          &stringLength, &stringBuffer, NULL);
#endif
	if(status) return nil;
	
#if TARGET_OS_IPHONE
  NSString *string = [[NSString alloc] initWithData:(IIBRIDGE id)stringData encoding:NSUTF8StringEncoding];
  CFRelease(stringData);
#else //OSX
  NSString *string = [[NSString alloc] initWithBytes:stringBuffer length:stringLength encoding:NSUTF8StringEncoding];
  SecKeychainItemFreeAttributesAndData(NULL, stringBuffer);
#endif
	return string;
}


- (BOOL)storeString:(NSString*)string forKey:(NSString*)key {
	if (!string)  {
		//Need to delete the Key
#if TARGET_OS_IPHONE
    NSDictionary *spec = [NSDictionary dictionaryWithObjectsAndKeys:(IIBRIDGE id)kSecClassGenericPassword, kSecClass,
                          key, kSecAttrAccount,[self serviceName], kSecAttrService, nil];
    
    return !SecItemDelete((IIBRIDGE CFDictionaryRef)spec);
#else //OSX
    SecKeychainItemRef item = NULL;
    OSStatus status = SecKeychainFindGenericPassword(NULL, (uint) [[self serviceName] lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [[self serviceName] UTF8String],
                                                     (uint) [key lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [key UTF8String],
                                                     NULL, NULL, &item);
    if(status) return YES;
    if(!item) return YES;
    return !SecKeychainItemDelete(item);
#endif
  } else {
#if TARGET_OS_IPHONE
    NSData *stringData = [string dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *spec = [NSDictionary dictionaryWithObjectsAndKeys:(IIBRIDGE id)kSecClassGenericPassword, kSecClass,
                          key, kSecAttrAccount,[self serviceName], kSecAttrService, nil];
    
    if(!string) {
      return !SecItemDelete((IIBRIDGE CFDictionaryRef)spec);
    }else if([self stringForKey:key]) {
      NSDictionary *update = [NSDictionary dictionaryWithObject:stringData forKey:(IIBRIDGE id)kSecValueData];
      return !SecItemUpdate((IIBRIDGE CFDictionaryRef)spec, (IIBRIDGE CFDictionaryRef)update);
    }else{
      NSMutableDictionary *data = [NSMutableDictionary dictionaryWithDictionary:spec];
      [data setObject:stringData forKey:(IIBRIDGE id)kSecValueData];
      return !SecItemAdd((IIBRIDGE CFDictionaryRef)data, NULL);
    }
#else //OSX
    SecKeychainItemRef item = NULL;
    OSStatus status = SecKeychainFindGenericPassword(NULL, (uint) [[self serviceName] lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [[self serviceName] UTF8String],
                                                     (uint) [key lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [key UTF8String],
                                                     NULL, NULL, &item);
    if(status) {
      //NO such item. Need to add it
      return !SecKeychainAddGenericPassword(NULL, (uint) [[self serviceName] lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [[self serviceName] UTF8String],
                                            (uint) [key lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [key UTF8String],
                                            (uint) [string lengthOfBytesUsingEncoding:NSUTF8StringEncoding],[string UTF8String],
                                            NULL);
    }
    
    if(item)
      return !SecKeychainItemModifyAttributesAndData(item, NULL, (uint) [string lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [string UTF8String]);
    
    else
      return !SecKeychainAddGenericPassword(NULL, (uint) [[self serviceName] lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [[self serviceName] UTF8String],
                                            (uint) [key lengthOfBytesUsingEncoding:NSUTF8StringEncoding], [key UTF8String],
                                            (uint) [string lengthOfBytesUsingEncoding:NSUTF8StringEncoding],[string UTF8String],
                                            NULL);
#endif
  }
}

#pragma mark -
#pragma mark Singleton Stuff

+ (PDKeychainBindingsController *)sharedKeychainBindingsController
{
  static dispatch_once_t onceQueue;
  
  dispatch_once(&onceQueue, ^{
    sharedInstance = [[self alloc] init];
  });
  
	return sharedInstance;
}

#pragma mark -
#pragma mark Business Logic

- (PDKeychainBindings *) keychainBindings {
  if (_keychainBindings == nil) {
    _keychainBindings = [[PDKeychainBindings alloc] init];
  }
  if (_valueBuffer==nil) {
    _valueBuffer = [[NSMutableDictionary alloc] init];
  }
  return _keychainBindings;
}

-(id) values {
  if (_valueBuffer==nil) {
    _valueBuffer = [[NSMutableDictionary alloc] init];
  }
  return _valueBuffer;
}

- (id)valueForKeyPath:(NSString *)keyPath {
  NSRange firstSeven=NSMakeRange(0, 7);
  if (NSEqualRanges([keyPath rangeOfString:@"values."],firstSeven)) {
    //This is a values keyPath, so we need to check the keychain
    NSString *subKeyPath = [keyPath stringByReplacingCharactersInRange:firstSeven withString:@""];
    NSString *retrievedString = [self stringForKey:subKeyPath];
    if (retrievedString) {
      if (![_valueBuffer objectForKey:subKeyPath] || ![[_valueBuffer objectForKey:subKeyPath] isEqualToString:retrievedString]) {
        //buffer has wrong value, need to update it
        [_valueBuffer setValue:retrievedString forKey:subKeyPath];
      }
    }
  }
  
  return [super valueForKeyPath:keyPath];
}


- (void)setValue:(id)value forKeyPath:(NSString *)keyPath {
  NSRange firstSeven=NSMakeRange(0, 7);
  if (NSEqualRanges([keyPath rangeOfString:@"values."],firstSeven)) {
    //This is a values keyPath, so we need to check the keychain
    NSString *subKeyPath = [keyPath stringByReplacingCharactersInRange:firstSeven withString:@""];
    NSString *retrievedString = [self stringForKey:subKeyPath];
    if (retrievedString) {
      if (![value isEqualToString:retrievedString]) {
        [self storeString:value forKey:subKeyPath];
      }
      if (![_valueBuffer objectForKey:subKeyPath] || ![[_valueBuffer objectForKey:subKeyPath] isEqualToString:value]) {
        //buffer has wrong value, need to update it
        [_valueBuffer setValue:value forKey:subKeyPath];
      }
    } else {
      //First time to set it
      [self storeString:value forKey:subKeyPath];
      [_valueBuffer setValue:value forKey:subKeyPath];
    }
  }
  [super setValue:value forKeyPath:keyPath];
}

@end
