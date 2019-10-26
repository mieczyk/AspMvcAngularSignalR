export class ReceivedMessage {
    constructor(public sender: string, public body: string) { }
}

export class SentMessage {
    static readonly recipientRegEx = /@(\w+):/;

    constructor(public body: string, public recipient?: string) { }

    /* 
     * Creates a new SentMessage instance object from a given string. 
     * Expects the following string formats:
     * "@RECIPIENT:MESSAGE"
     * "MESSAGE"
     */ 
    static fromString(messageString: string) {
        if (!messageString) {
            return new SentMessage(null);
        }

        let body = messageString;
        let recipient = null;

        if (messageString.startsWith('@')) {
            let matches = SentMessage.recipientRegEx.exec(messageString);

            if (matches) {
                recipient = matches[1];
                body = messageString.replace(matches[0], '').trim();
            }
        }

        return new SentMessage(body, recipient);
    }

    isEmpty() {
        return this.body === null || this.body.replace(/\s+/g, '') === '';
    }
}