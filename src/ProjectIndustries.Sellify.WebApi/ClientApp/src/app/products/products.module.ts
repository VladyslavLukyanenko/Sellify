import {NgModule} from "@angular/core";
import {ProductsTablePageComponent} from "./components/products-table-page/products-table-page.component";
import {SharedModule} from "../shared/shared.module";
import {ProductsRoutesModule} from "./products-routes.module";
import {QuillModule} from "ngx-quill";
import {ProductEditorDialogComponent} from "./components/product-editor-dialog/product-editor-dialog.component";
import {ProductAttributeEditorComponent} from "./components/product-attribute-editor/product-attribute-editor.component";
import {PictureUploaderComponent} from "./components/picture-uploader/picture-uploader.component";


@NgModule({
  declarations: [
    ProductsTablePageComponent,
    ProductEditorDialogComponent,
    ProductAttributeEditorComponent,
    PictureUploaderComponent],
  imports: [
    SharedModule,
    ProductsRoutesModule,
    QuillModule
  ]
})
export class ProductsModule {
}
