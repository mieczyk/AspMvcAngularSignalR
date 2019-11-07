import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SignalRModule } from 'ng2-signalr';
import { SignalRConfiguration } from 'ng2-signalr';

import { ChatWindow } from './chat-window.component';
import { ChatSender } from './chat-sender.component';

export function createSignalRConfig(): SignalRConfiguration {
    const config = new SignalRConfiguration();
    config.hubName = 'messagesHub';
    return config;
}

@NgModule({
  declarations: [
    ChatWindow, ChatSender
  ],
  imports: [
      BrowserModule,
      SignalRModule.forRoot(createSignalRConfig)
  ],
  bootstrap: [ChatWindow, ChatSender]
})
export class AppModule { }
