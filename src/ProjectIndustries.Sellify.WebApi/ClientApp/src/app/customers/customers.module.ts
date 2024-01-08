import {NgModule} from "@angular/core";
import {CustomersTablePageComponent} from "./components/customers-table-page/customers-table-page.component";
import {SharedModule} from "../shared/shared.module";
import {CustomersRoutingModule} from "./customers-routing.module";


@NgModule({
  declarations: [CustomersTablePageComponent],
  imports: [
    SharedModule,
    CustomersRoutingModule
  ]
})
export class CustomersModule { }
