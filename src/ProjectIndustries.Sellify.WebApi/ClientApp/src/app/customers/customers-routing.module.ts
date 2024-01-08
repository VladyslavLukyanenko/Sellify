import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {CustomersTablePageComponent} from "./components/customers-table-page/customers-table-page.component";

const routes: Routes = [
  {path: "", pathMatch: "full", component: CustomersTablePageComponent}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class CustomersRoutingModule {
}
