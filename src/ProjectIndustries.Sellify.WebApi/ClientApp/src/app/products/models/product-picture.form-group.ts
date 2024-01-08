import {FormControl, FormGroup} from "@angular/forms";

export class ProductPictureFormGroup extends FormGroup {
  constructor() {
    super({
      position: new FormControl(),
      source: new FormControl()
    });
  }
}
