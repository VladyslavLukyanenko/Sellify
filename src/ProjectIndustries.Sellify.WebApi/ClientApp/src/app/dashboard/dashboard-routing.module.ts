import {RouterModule, Routes} from "@angular/router";
import {AnalyticsPageComponent} from "./components/analytics-page/analytics-page.component";
import {NgModule} from "@angular/core";

const routes: Routes = [
  {path: "", pathMatch: "full", component: AnalyticsPageComponent}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class DashboardRoutingModule {
}
