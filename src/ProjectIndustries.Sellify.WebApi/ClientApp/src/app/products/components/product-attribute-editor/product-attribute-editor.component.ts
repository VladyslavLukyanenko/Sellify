import {ChangeDetectionStrategy, Component, OnInit, Output, EventEmitter} from "@angular/core";
import {AttributeType} from "../../../sellify-api";
import {ControlContainer} from "@angular/forms";
import {ProductAttributeFormGroup} from "../../models/product-attribute.form-group";
import {KeyValuePair} from "../../../core/models/key-value-pair.model";

@Component({
  selector: "app-product-attribute-editor",
  templateUrl: "./product-attribute-editor.component.html",
  styleUrls: ["./product-attribute-editor.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductAttributeEditorComponent implements OnInit {
  @Output() removeClick = new EventEmitter<void>();

  types: KeyValuePair<AttributeType>[] = [
    {
      key: AttributeType.Color,
      value: AttributeType.Color
    },
    {
      key: AttributeType.Text,
      value: AttributeType.Text
    }
  ];

  constructor(private controlContainer: ControlContainer) {
  }

  get form(): ProductAttributeFormGroup {
    return this.controlContainer.control as ProductAttributeFormGroup;
  }

  ngOnInit(): void {
  }

}
