import {Injectable} from "@angular/core";
// import {TokenService} from "./token.service";
import {BehaviorSubject, Observable} from "rxjs";
import {distinctUntilChanged} from "rxjs/operators";
import {ClaimNames} from "../models/claim-names.model";
import {Identity} from "./identity.model";


const getValues = (token: any, key: string): any[] => {
  const values = token[key] || [];
  return typeof values === "string"
    ? [values]
    : values;
};

@Injectable({
  providedIn: "root"
})
export class IdentityService {
  private readonly user$: BehaviorSubject<Identity | null>;

  readonly currentUser$: Observable<Identity | null>;

  constructor(/*private readonly tokenService: TokenService*/) {
    this.user$ = new BehaviorSubject<Identity | null>(null);
    this.currentUser$ = this.user$
      .asObservable()
      .pipe(
        distinctUntilChanged((left, right) => !!left && !!right && left.id === right.id)
      );

    // tokenService.encodedAccessToken$.subscribe(() => {
    //   const tokenData = tokenService.decodedAccessToken;
    //   if (!tokenData) {
    //     this.user$.next(null);
    //     return;
    //   }
    //
    //   const user: Identity = {
    //     email: tokenData[ClaimNames.email],
    //     id: +tokenData[ClaimNames.id],
    //     avatar: tokenData[ClaimNames.avatar],
    //     name: tokenData[ClaimNames.name],
    //     discordId: +tokenData[ClaimNames.discordId],
    //     discriminator: tokenData[ClaimNames.discriminator],
    //     roleNames: getValues(tokenData, ClaimNames.roleName),
    //     roleIds: getValues(tokenData, ClaimNames.roleId).map(r => +r),
    //     permissions: getValues(tokenData, ClaimNames.permission),
    //   };
    //
    //   this.user$.next(user);
    // });
  }

  get currentUser(): Identity | null {
    return this.user$.getValue();
  }
}
