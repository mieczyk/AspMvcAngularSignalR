import { Component } from '@angular/core';

@Component({
    selector: 'chat-sender',
    templateUrl: './chat-sender.component.html',
    styles: []
})
export class ChatSender {
    public message: string;

    public sendMessage(message: string) {
        this.message = message;
    }
}