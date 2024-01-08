import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-store-host',
  templateUrl: './store-host.component.html',
  styleUrls: ['./store-host.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class StoreHostComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
