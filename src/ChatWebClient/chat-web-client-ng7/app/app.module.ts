import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { ChatWindow } from './chat-window.component';

@NgModule({
  declarations: [
    ChatWindow
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [ChatWindow]
})
export class AppModule { }
