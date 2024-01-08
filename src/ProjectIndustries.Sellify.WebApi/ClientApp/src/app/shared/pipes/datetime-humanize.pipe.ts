import { Pipe, PipeTransform } from "@angular/core";
import {DateUtil} from "../../core/services/date.util";

@Pipe({
  name: "datetimeHumanize"
})
export class DatetimeHumanizePipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): string {
    return DateUtil.humanizeDateTime(value);
  }

}
