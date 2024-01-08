import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {OrdersTablePageComponent} from "./components/orders-table-page/orders-table-page.component";

const routes: Routes = [
  {path: "", pathMatch: "full", component: OrdersTablePageComponent}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class OrdersRoutingModule {
}
