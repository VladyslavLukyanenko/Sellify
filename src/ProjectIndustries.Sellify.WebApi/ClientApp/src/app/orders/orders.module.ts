import {NgModule} from "@angular/core";
import {OrdersTablePageComponent} from "./components/orders-table-page/orders-table-page.component";
import {SharedModule} from "../shared/shared.module";
import {OrdersRoutingModule} from "./orders-routing.module";


@NgModule({
  declarations: [OrdersTablePageComponent],
  imports: [
    SharedModule,
    OrdersRoutingModule
  ]
})
export class OrdersModule { }
