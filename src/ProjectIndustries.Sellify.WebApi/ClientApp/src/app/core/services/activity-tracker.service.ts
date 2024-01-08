import {Injectable} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class ActivityTrackerService {
  private pendingRequests = new BehaviorSubject<any[]>([]);

  pendingRequestsCount$: Observable<number>;

  constructor() {
    this.pendingRequestsCount$ = this.pendingRequests.asObservable()
      .pipe(map(r => r.length));
  }

  push(r: any): void {
    this.pendingRequests.next([
      ...this.pendingRequests.getValue(),
      r
    ]);
  }

  remove(r: any): void {
    const pendingRequests = this.pendingRequests.getValue().slice();
    const idx = pendingRequests.indexOf(r);
    if (idx !== -1) {
      pendingRequests.splice(idx, 1);
      this.pendingRequests.next(pendingRequests);
    }
  }
}
