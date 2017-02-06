|   | Commands | Events |
|---|:--------:|:------:|
| Marker Interface | `ICommand` | `IEvent` |
| Logical Senders | Many | 1 |
| Logical Receivers | 1 | Many (or none) |
| Purpose | "Please do something" | "Something has happened" |
| Naming (Tense) | Imperative | Past |
| Examples | `PlaceOrder`<br/>`ChargeCreditCard` | `OrderPlaced`<br/>`CreditCardCharged` |
| Coupling Style | Tight | Loose |
