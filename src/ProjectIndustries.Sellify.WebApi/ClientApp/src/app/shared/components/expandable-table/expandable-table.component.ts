import {Component, OnInit, ChangeDetectionStrategy, Input, TemplateRef, Output, EventEmitter} from "@angular/core";
import {InfiniteScrollDataSourceBase} from "../../../core/services/infinite-scroll.data-source-base";

@Component({
  selector: "app-expandable-table",
  templateUrl: "./expandable-table.component.html",
  styleUrls: ["./expandable-table.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExpandableTableComponent implements OnInit {
  @Input() dataSource: InfiniteScrollDataSourceBase<any>;
  @Input() columns: ColumnDef[];
  @Input() itemSize: number;
  @Input() trackBy: (idx: number, item: any) => any;
  @Input() expandedRowTmpl: TemplateRef<any>;
  @Input() tableClass: string;
  @Input() multiSelect = true;

  @Output() expandToggled = new EventEmitter<{ expanded: boolean, data: any }>();

  expandedRow: any;

  constructor() {
  }

  ngOnInit(): void {
  }

  toggleExpandDetails(row: any): void {
    const shouldCollapse = this.expandedRow === row;
    this.expandToggled.emit({expanded: !shouldCollapse, data: row});
    if (shouldCollapse) {
      this.expandedRow = null;
      return;
    }

    this.expandedRow = row;
  }

  isValueTemplated(def: ColumnDef): boolean {
    return def.value instanceof TemplateRef;
  }

  extractValue(def: ColumnDef, item: any): string {
    if (this.isValueTemplated(def)) {
      throw new Error("Can't extract value from template");
    }

    if (typeof def.value === "function") {
      return def.value(item);
    }

    return def.value as string;
  }
}

export interface ColumnDef<T = any> {
  title: string;
  classes?: ColumnClasses;
  value: ((item: T) => string) | string | TemplateRef<any>;
}

export interface ColumnClasses {
  headerCol?: string;
  dataCol?: string;
}
