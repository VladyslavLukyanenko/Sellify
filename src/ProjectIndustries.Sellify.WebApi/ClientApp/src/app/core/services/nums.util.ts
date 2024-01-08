export class NumsUtil {
  static parseOrNull(n: any): null | number {
    return n == null || isNaN(+n) ? null : +n;
  }

  static toLeftPaddedStr(v: number, placeholders: string = "00"): string {
    const str = String(v);
    return placeholders.substr(0, placeholders.length - str.length) + str;
  }
}
