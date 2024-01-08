import {NgModule} from "@angular/core";
import {AnalyticsPageComponent} from "./components/analytics-page/analytics-page.component";
import {SharedModule} from "../shared/shared.module";
import {DashboardRoutingModule} from "./dashboard-routing.module";
import { StatsEntryComponent } from "./components/stats-entry/stats-entry.component";


import { NgxEchartsModule } from "ngx-echarts";
// Import the echarts core module, which provides the necessary interfaces for using echarts.
import * as echarts from "echarts/core";
// Import bar charts, all with Chart suffix
import {
  LineChart
} from "echarts/charts";
import {
  TitleComponent,
  TooltipComponent,
  GridComponent,
} from "echarts/components";
// Import the Canvas renderer, note that introducing the CanvasRenderer or SVGRenderer is a required step
import {
  CanvasRenderer
} from "echarts/renderers";
import "echarts/theme/macarons.js";

echarts.use(
  [TitleComponent, TooltipComponent, GridComponent, LineChart, CanvasRenderer]
);
@NgModule({
  declarations: [AnalyticsPageComponent, StatsEntryComponent],
  imports: [
    SharedModule,
    DashboardRoutingModule,

    NgxEchartsModule.forRoot({ echarts }),
  ]
})
export class DashboardModule { }
