#import "UnityStoreKit.h"

@implementation UnityStoreKit

#if defined(__cplusplus)

extern "C"{

#endif
     NSString *appId = @"12345";

    void _goComment()

    {

        if([SKStoreReviewController respondsToSelector:@selector(requestReview)]) {// iOS 10.3 以上支持

            [SKStoreReviewController requestReview];

        } else { // iOS 10.3 之前的使用这个

            // NSString *appId = @"1234567890"; //项目在苹果后台的appid

            NSString  * nsStringToOpen = [NSString  stringWithFormat: @"itms-apps://itunes.apple.com/app/id%@?action=write-review",appId];//替换为对应的APPID

            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:nsStringToOpen]];

        }

    }
	
	void setAppId(const char *id)
	{
		appId = [[NSString alloc] initWithUTF8String:id];
	}

#if defined(__cplusplus)

}

#endif



@end