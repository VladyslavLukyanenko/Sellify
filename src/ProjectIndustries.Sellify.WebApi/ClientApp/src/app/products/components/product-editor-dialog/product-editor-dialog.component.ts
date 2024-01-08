import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewEncapsulation
} from "@angular/core";
import {
  CategoriesService,
  CategoryGraphData,
  ProductData,
  ProductsService,
  ProductType,
  SaveProductCommand
} from "../../../sellify-api";
import {BehaviorSubject} from "rxjs";
import {DisposableComponentBase} from "../../../shared/components/disposable.component-base";
import {map} from "rxjs/operators";
import {ProductFormGroup} from "../../models/product.form-group";
import {FormUtil} from "../../../core/services/form.util";
import {NotificationService} from "../../../core/services/notifications/notification.service";
import {OperationStatusMessage} from "../../../core/services/notifications/messages.model";
import {KeyValuePair} from "../../../core/models/key-value-pair.model";

@Component({
  selector: "app-product-editor-dialog",
  templateUrl: "./product-editor-dialog.component.html",
  styleUrls: ["./product-editor-dialog.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class ProductEditorDialogComponent extends DisposableComponentBase implements OnInit {
  @Input() isVisible: boolean;
  @Input() productId?: number;
  @Output() isVisibleChange = new EventEmitter<boolean>();
  @Output() saved = new EventEmitter<ProductData>();

  form = new ProductFormGroup();
  productTypes: KeyValuePair<ProductType>[] = [
    {key: ProductType.Goods, value: ProductType.Goods},
    {key: ProductType.Services, value: ProductType.Services}
  ];

  categories$ = new BehaviorSubject<CategoryGraphData[]>([]);
  uploadedPic$ = new BehaviorSubject<string>(null);

  constructor(private categoriesService: CategoriesService,
              private notificationService: NotificationService,
              private productService: ProductsService) {
    super();
  }

  async ngOnInit(): Promise<void> {
    this.asyncTracker.isLoading$
      .pipe(
        this.untilDestroy()
      )
      .subscribe(isLoading => isLoading ? this.form.disable() : this.form.enable());

    const categories = await this.asyncTracker.executeAsAsync(
      this.categoriesService.categoriesGetCategoriesGraph().pipe(map(_ => _.payload))
    );

    this.categories$.next(categories);
    if (this.productId) {
      const toUpdate = await this.asyncTracker.executeAsAsync(
        this.productService.productsGetById(this.productId).pipe(map(_ => _.payload))
      );

      this.form.patchValue(toUpdate, {emitEvent: false});
    }
  }

  async saveProduct(): Promise<void> {
    if (this.form.invalid) {
      FormUtil.validateAllFormFields(this.form);
      return;
    }

    const cmd: SaveProductCommand = this.form.value;
    try {
      const op = cmd.id
        ? this.productService.productsUpdate(cmd.id, cmd)
        : this.productService.productsCreate(cmd);

      await this.asyncTracker.executeAsAsync(op);
      this.saved.emit(cmd);
      const msg = cmd.id ? OperationStatusMessage.UPDATED : OperationStatusMessage.CREATED;
      this.notificationService.success(msg);
    } catch (e) {
      this.notificationService.error(OperationStatusMessage.FAILED);
      console.error(e);
    }
  }

  uploadPictures(file: File): void {
    const reader = new FileReader();
    reader.addEventListener("loadend", e => {
      const dataUri = reader.result as string;
      const uploadedFile = {
        name: file.name,
        content: dataUri,
        contentType: file.type,
        length: file.size
      };

      this.form.uploadedPictureCtrl.patchValue(uploadedFile, {emitEvent: false});
      this.uploadedPic$.next(dataUri);
    });

    reader.readAsDataURL(file);
  }

  removeUploadedPicture(): void {
    this.form.uploadedPictureCtrl.patchValue({}, {emitEvent: false});
    this.form.pictureCtrl.setValue(null);
    this.uploadedPic$.next(null);
  }
}
