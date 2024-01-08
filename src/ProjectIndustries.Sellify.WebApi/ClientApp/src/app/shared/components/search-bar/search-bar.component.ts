import {Component, OnInit, ChangeDetectionStrategy, Input, EventEmitter, Output} from "@angular/core";

@Component({
  selector: "app-search-bar",
  templateUrl: "./search-bar.component.html",
  styleUrls: ["./search-bar.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SearchBarComponent {
  @Input() placeholder: string;
  @Input() value: string;
  @Output() valueChange = new EventEmitter<string>();
}
