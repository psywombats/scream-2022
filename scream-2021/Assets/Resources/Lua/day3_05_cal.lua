speak("Dr. Cooper", "Ahh if it isn't Tess. There you are!")
speak("Lia", "Hey Cal. You already know Tess?")
speak("Dr. Cooper", "Yep, I know she's late for our 4 o'clock. Haha! This way, Tess. And catch you later, ummm...")
speak("Lia", "I'm Lia.")
speak("Dr. Cooper", "Right, Lia. See you Lia! Haha!")

speak("Dr. Cooper", "Tess. Tess Tess Tess Tess Tess.")
speak("Dr. Cooper", "I'm going to ask a series of questions to assess your cognitive state and powers of specific recall.")
speak("Tess", "Dr. Cooper, how long have I been in Ward #6?")
speak("Dr. Cooper", "Tess Tess Tess. What a question.")
speak("Tess", "Longer than a day?")
speak("Dr. Cooper", "You crack me up Tess. Tess Tess Tess. You've been here almost longer than me. I think.")
speak("Tess", "Everyone in the ward forgot me.")
speak("Dr. Cooper", "Well it is an N9 ward after all. Haha! Now, are you comfy? Are you ready?")
speak("Tess", "This doesn't seem important right now.")
speak("Dr. Cooper", "Fair enough. I've already broken through, but humor me, for old time's sake.")
speak("Dr. Cooper", "Now then... What was the first question?")

choice("What is my name?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Tess, I think.")
else
    speak("Dr. Cooper", "That makes two of us!")
end

speak("Dr. Cooper", "Next question... The second question.")
speak("Dr. Cooper", "Could you please tell me the second question?")

choice("What is the current year?", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "That's one of many smaller, pettier details that I've exchanged for truer knowledge of the nature of reality.")
    speak("Dr. Cooper", "Or something like that!")
else
    speak("Dr. Cooper", "Spooky. I don't know it either.")
end

speak("Dr. Cooper", "Ahaha. Just pulling your leg. Of course I know that.")
speak("Dr. Cooper", "Now, the moment of truth. The razor's edge! The final question. The final answer.")
speak("Dr. Cooper", "TELL ME, OH TELL ME, WHAT IS THE PURPOSE OF MY LIFE?")
choice("There is none.", "I don't know.")
if choice_result == 0 then
    speak("Dr. Cooper", "Really? Is that really the answer?")
    speak("Tess", "I do not know or care.")
else
    speak("Dr. Cooper", "No. No! No no no!")
end

speak("Dr. Cooper", "Tess. 2876. Cecily. Whatever your name is. I need you!")
speak("Dr. Cooper", "Tell me what it is I was meant to do!")
speak("Tess", "Who do you think I am?")
speak("Dr. Cooper", "You've done it! You're the culmination of everything we've been working towards!")
speak("Dr. Cooper", "Tell me the future. Tell me it wasn't all for nothing, all of that, that...")
speak("Dr. Cooper", "Ahem.")
speak("Dr. Cooper", "Thanks for bearing with me. We'll move on and take your vitals and do a basic checkup from here.")
speak("Dr. Cooper", "Next time please think a little harder about the questions, okay? It's really important for my research.")
