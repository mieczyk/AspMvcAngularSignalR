﻿import { Component } from '@angular/core';
import { SignalR } from 'ng2-signalr';

import { SentMessage } from './models/message.model';

@Component({
    selector: 'chat-sender',
    templateUrl: './chat-sender.component.html',
    styles: []
})
export class ChatSender {
    constructor(private _signalR: SignalR) {}

    public sendMessage(messageString: string) {
        let message = SentMessage.fromString(messageString);

        if (!message.isEmpty()) {
            this._signalR.connect().then((connection) => {
                connection.invoke('Send', message);
            });
        }
    }
}