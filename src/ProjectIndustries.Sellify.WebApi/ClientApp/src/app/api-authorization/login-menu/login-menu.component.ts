import {Component, OnInit} from "@angular/core";
import {AuthorizeService} from "../authorize.service";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";

import {MenuItem} from "primeng/api";
import fallbackPic from "../../../assets/no-user-picture.svg";

const fallbackPlan = "Free";

@Component({
  selector: "app-login-menu",
  templateUrl: "./login-menu.component.html",
  styleUrls: ["./login-menu.component.less"]
})
export class LoginMenuComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  user$: Observable<UserInfo>;

  loggedInMenuItems: MenuItem[] = [
    {
      label: "Profile",
      routerLink: ["/authentication/profile"]
    },
    {
      label: "Log out",
      routerLink: ["/authentication/logout"],
      state: {local: true}
    }
  ];

  constructor(private authorizeService: AuthorizeService) {
  }

  ngOnInit(): void {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
    this.user$ = this.authorizeService.getUser().pipe(map(u => ({
      fname: u.given_name,
      lname: u.family_name,
      pic: u.picture || fallbackPic,
      plan: u.sub_type || fallbackPlan
    })));
  }
}


interface UserInfo {
  fname: string;
  lname: string;
  plan?: string;
  pic: string;
}
