import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SignalRModule, SignalRConfiguration } from 'ng2-signalr';

import { ChatWindow } from './chat-window.component';

export function createSignalRConfig(): SignalRConfiguration {
    const config = new SignalRConfiguration();
    config.hubName = 'messagesHub';
    return config;
}

@NgModule({
  declarations: [
    ChatWindow
  ],
  imports: [
      BrowserModule,
      SignalRModule.forRoot(createSignalRConfig)
  ],
  providers: [],
  bootstrap: [ChatWindow]
})

export class AppModule { }
