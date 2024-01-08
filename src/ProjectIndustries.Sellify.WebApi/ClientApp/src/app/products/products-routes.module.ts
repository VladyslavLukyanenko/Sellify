import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {ProductsTablePageComponent} from "./components/products-table-page/products-table-page.component";

const routes: Routes = [
  {path: "", pathMatch: "full", component: ProductsTablePageComponent}
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ProductsRoutesModule {
}
