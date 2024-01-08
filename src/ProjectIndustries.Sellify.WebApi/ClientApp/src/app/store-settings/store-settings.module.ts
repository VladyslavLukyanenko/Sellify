import {NgModule} from "@angular/core";
import {SettingsPageComponent} from "./components/settings-page/settings-page.component";
import {SharedModule} from "../shared/shared.module";
import {StoreSettingsRoutingModule} from "./store-settings-routing.module";

@NgModule({
  declarations: [SettingsPageComponent],
  imports: [
    SharedModule,
    StoreSettingsRoutingModule
  ]
})
export class StoreSettingsModule {
}
