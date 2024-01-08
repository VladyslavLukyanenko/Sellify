import {Pipe, PipeTransform} from "@angular/core";
import {DateUtil} from "../../core/services/date.util";

@Pipe({
  name: "dateHumanize"
})
export class DateHumanizePipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): string {
    return DateUtil.humanizeDate(value);
  }

}
