import { Component } from '@angular/core';

import { SignalRService } from './signalr.service';

@Component({
    selector: 'chat-sender',
    templateUrl: './chat-sender.component.html',
    styles: []
})
export class ChatSender {
    constructor(private signalRService: SignalRService) {}

    public sendMessage(messageString: string) {
        if (messageString) {
            this.signalRService.getOpenConnection().then((connection) => {
                connection.invoke('Send', messageString);
            });
        }
    }
}