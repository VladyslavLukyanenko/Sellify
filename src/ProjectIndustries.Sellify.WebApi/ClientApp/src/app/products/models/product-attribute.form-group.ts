import {FormControl, FormGroup, Validators} from "@angular/forms";
import {ProductAttribute} from "../../sellify-api";

export class ProductAttributeFormGroup extends FormGroup {
  constructor(data?: ProductAttribute) {
    data = data || {};
    super({
      type: new FormControl(data.type, [Validators.required]),
      name: new FormControl(data.name, [Validators.required]),
      value: new FormControl(data.value, [Validators.required]),
    });
  }

  get typeCtrl(): FormControl {
    return this.get("type") as FormControl;
  }

  get nameCtrl(): FormControl {
    return this.get("name") as FormControl;
  }

  get valueCtrl(): FormControl {
    return this.get("value") as FormControl;
  }
}
