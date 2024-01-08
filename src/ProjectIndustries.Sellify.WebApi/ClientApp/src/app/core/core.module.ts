import {RouterModule} from "@angular/router";
import {NgModule, Optional, SkipSelf} from "@angular/core";

import {LayoutComponent} from "./components/layout/layout.component";

import {throwIfAlreadyLoaded} from "./module-import-guard";
import {SharedModule} from "../shared/shared.module";

import {MainMenuComponent} from "./components/main-menu/main-menu.component";
import {ToolbarComponent} from "./components/toolbar/toolbar.component";
import {ProfileWidgetComponent} from "./components/profile-widget/profile-widget.component";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {SidebarComponent} from "./components/sidebar/sidebar.component";
import {LogoComponent} from "./components/logo/logo.component";
import {NotFoundComponent} from "./components/not-found/not-found.component";
import {ApiModule, Configuration} from "../sellify-api";
import {SubscriptionPlanWidgetComponent} from "./components/subscription-plan-widget/subscription-plan-widget.component";
import {ApiAuthorizationModule} from "../api-authorization/api-authorization.module";
import {environment} from "../../environments/environment";

@NgModule({
  imports: [
    RouterModule,
    BrowserAnimationsModule,


    ApiAuthorizationModule,
    SharedModule,
    ApiModule
  ],
  declarations: [
    LayoutComponent,

    MainMenuComponent,
    ToolbarComponent,
    ProfileWidgetComponent,
    SidebarComponent,
    LogoComponent,
    NotFoundComponent,
    SubscriptionPlanWidgetComponent
  ],
  exports: [LayoutComponent],
  providers: [
    {
      provide: Configuration,
      useFactory: () => new Configuration({
        basePath: environment.apiHostUrl,
        credentials: ({
          Bearer: () => {
            return null;
            // const token = tokenService.encodedAccessToken!;
            // return token && "Bearer " + token || null;
          }
        })
      }),
      // deps: [TokenService]
    },
  ],
  entryComponents: []
})
export class CoreModule {
  constructor(
    @Optional()
    @SkipSelf()
      parentModule: CoreModule
  ) {
    throwIfAlreadyLoaded(parentModule, "CoreModule");
  }
}
