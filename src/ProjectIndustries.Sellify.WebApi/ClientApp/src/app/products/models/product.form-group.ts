import {FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import {ProductData} from "../../sellify-api";
import {ProductAttributeFormGroup} from "./product-attribute.form-group";
import {Base64FileFormGroup} from "../../shared/models/base64-file.resource";

export class ProductFormGroup extends FormGroup {
  constructor(data?: ProductData) {
    data = data || {};
    data.attributes = data.attributes || [];
    super({
      id: new FormControl(data.id),
      sku: new FormControl(data.sku, [Validators.required]),
      title: new FormControl(data.title, [Validators.required]),
      stock: new FormControl(data.stock, [Validators.required]),
      content: new FormControl(data.content, [Validators.required]),
      excerpt: new FormControl(data.excerpt, [Validators.required]),
      type: new FormControl(data.type, [Validators.required]),
      price: new FormControl(data.price, [Validators.required]),
      categoryId: new FormControl(data.categoryId, [Validators.required]),
      attributes: new FormArray(data.attributes.map(a => new ProductAttributeFormGroup(a))),
      picture: new FormControl(data.picture),
      uploadedPicture: new Base64FileFormGroup(),
    });
  }

  get idCtrl(): FormControl {
    return this.get("id") as FormControl;
  }

  get skuCtrl(): FormControl {
    return this.get("sku") as FormControl;
  }

  get titleCtrl(): FormControl {
    return this.get("title") as FormControl;
  }

  get stockCtrl(): FormControl {
    return this.get("stock") as FormControl;
  }

  get contentCtrl(): FormControl {
    return this.get("content") as FormControl;
  }

  get excerptCtrl(): FormControl {
    return this.get("excerpt") as FormControl;
  }

  get typeCtrl(): FormControl {
    return this.get("type") as FormControl;
  }

  get priceCtrl(): FormControl {
    return this.get("price") as FormControl;
  }

  get categoryIdCtrl(): FormControl {
    return this.get("categoryId") as FormControl;
  }

  get attributesCtrl(): FormArray {
    return this.get("attributes") as FormArray;
  }

  get pictureCtrl(): FormControl {
    return this.get("picture") as FormControl;
  }

  get uploadedPictureCtrl(): Base64FileFormGroup {
    return this.get("uploadedPicture") as Base64FileFormGroup;
  }

  addEmptyAttribute(): void {
    this.attributesCtrl.push(new ProductAttributeFormGroup());
  }

  patchValue(value: ProductData, options?: { onlySelf?: boolean; emitEvent?: boolean }): void {
    super.patchValue(value, options);
    if (value.attributes?.length) {
      const attrs = value.attributes.map(a => new ProductAttributeFormGroup(a));
      for (const a of attrs) {
        this.attributesCtrl.push(a);
      }
    }
  }
}
