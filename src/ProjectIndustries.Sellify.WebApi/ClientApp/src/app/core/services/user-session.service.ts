import {Injectable} from "@angular/core";
import {AnalyticsService} from "../../sellify-api";

const sessionIdStoreKey = "gg.sellify.session.id";

@Injectable({
  providedIn: "root"
})
export class UserSessionService {
  constructor(private analyticsService: AnalyticsService) {
  }

  async refreshSession(): Promise<void> {
    const oldId = localStorage.getItem(sessionIdStoreKey);
    const r = await this.analyticsService.analyticsRefreshOrCreateSession(oldId).toPromise();
    localStorage.setItem(sessionIdStoreKey, r.payload);
  }
}
