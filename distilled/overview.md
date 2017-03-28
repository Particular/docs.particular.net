---
title: NServiceBus Distilled: An Overview
summary: An overview of NServiceBus for the non-technical reader.
---

What's the big deal about NServiceBus? There are a lot of different aspects, but it all comes down to **asynchronous messaging**, and how to do it properly.

So what does that mean?

## Storytime

Imagine that you decide to run a note-taking business. People will call you up on the phone, and they'll tell you something they want to remember later. You'll write that down in your notebook so that if they call again, you can read their note back to them.

Things are running great for awhile, until your phone rings one day and you realize that you left your notebook at home.

Now what?

Your memory is terrible; there's no way you'll remember the note when you get home, so you have no choice but to apologize to the caller and tell them that you can't take down their note right now. Maybe they should try again later, you tell them.

## Real life

This is what it's like to be a web application running on a web server. You can't remember anything yourself; you need a database (the notepad) to store information in. But as reliable as this notepad is, it can never be perfect, thanks to Murphy's Law and a [bunch of other reasons](http://en.wikipedia.org/wiki/Fallacies_of_distributed_computing).

In the story, you forgot your notebook at home. This is what it would be like if the database were offline - you would have no access to your notebook. But in reality, there are a host of other things that could go wrong. The details are unimportant - the takeaway is that any problem with your database could leave you unable to properly serve your customers.

## It gets worse

Integrations with web services are especially tricky. Imagine that in addition to your note-taking service, you also offered your customers the ability to book reservations at local restaurants. The customer tells you where they would like to eat and at what time, and then you put them on hold to call the restaurant yourself.

But sometimes, the host at the restaurant can't get to the phone quickly enough, or it takes you too long to look up the restaurant's phone number, and if it takes longer than 30 seconds to get a reservation, *your phone automatically hangs up on your customer*.

This is what it's like to integrate with external web services. In our example, you may or may not have been able to get a reservation for your customer, but they have no idea if the reservation was successful or not, because your phone hung up on them. They could show up at the restaurant and have a nice meal, or they might not even get a table; they have no way to know.

## Async to the rescue

Imagine how much better all of this would work out if, instead of your customers calling you on the phone, they sent you a text message instead.

For your note-taking service, it wouldn't matter if you forgot your notebook at home. You would have the text message saved in your phone. When you got home, you could take the messages from your phone and copy them into the notebook. The customer would not have to wait for you to write down the message in the notebook before they hung up the phone - they could just trust that, eventually, the message would get written down and, no matter what, it would be remembered.

In the same way, if your customer sent you a text message to make a hotel reservation instead of a phone call, the customer could go about their business knowing that they would certainly get a table at the restaurant, because they would trust you to keep calling the restuarant again and again until you succeeded in securing a reservation. Or at the very least, you could text them back if it turned out that the restaurant was completely booked.

## The big picture

This difference in communication style is the primary driver of what makes NServiceBus special.

**Synchronous communication**, like a phone call, is blocking. This means that when you make a call, you must wait for the person on the other end to answer, and then talk to them in real time. The answerer also must be available right away, and can't put off the conversation.

**Asynchronous communication** (often abbreviated async) is more like a text message. You send it and forget about it, confident that it will ultimately be received and read, although that might not happen immediately.

Most developers are very familiar with synchronous forms of communication. Not as many are fluent with asynchronous methods of communication, in part because it's harder to do properly.

Synchronous communication is technically easy to do, but projects that use it exclusively commonly fail, because they fall prey to the inherent problems with synchronous communication patterns alluded to above.

Asynchronous comunication is harder to do. It's not really as easy as sending a text message, if you're starting from square one.

NServiceBus, at a very high level, exists to make asynchronous communication in software projects as easy as possible, and to guide developers on how to do it correctly.
