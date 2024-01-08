import {Component, OnInit, ChangeDetectionStrategy, Output, EventEmitter} from "@angular/core";

@Component({
  selector: "app-picture-uploader",
  templateUrl: "./picture-uploader.component.html",
  styleUrls: ["./picture-uploader.component.less"],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PictureUploaderComponent implements OnInit {
  @Output() fileUploaded = new EventEmitter<File>();

  constructor() {
  }

  ngOnInit(): void {
  }


  uploadPictures(e: DragEvent): void {
    e.preventDefault();

    if (!e.dataTransfer.files.length) {
      return;
    }

    this.fileUploaded.emit(e.dataTransfer.files[0]);
  }
}
