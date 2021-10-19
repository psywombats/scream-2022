if getSwitch('day1_05_intro_nadine') then
    speak("Nadine", "And stick close to Tess. She's great!", 'd1_nadine0')
    return
end

speak("Nadine", "Hey there! Er, it's uh, errr...", 'd1_nadine0')
speak("Tess", "It's Tess.")
speak("Nadine", "Hey Tess. Right! And you, er, if you're with Tess, you must be Cecily?", 'd1_nadine0')
speak("Lia", "I'm, uhm, Lia. I'm going to be roommates with Tess.", 'lia_bot')
speak("Nadine", "Ohh, okay. So I haven't met you before. Exciting!", 'd1_nadine0')
speak("Nadine", "Sorry if you have to introduce yourself again. I'm Nadine, and I'm really bad with names. And faces.", 'd1_nadine0')
speak("Lia", "I don't mind.", 'lia_bot')
speak("Nadine", "And if I could find my camera, I'd love to take your picture.", 'd1_nadine0')
speak("Lia", "You're a photographer? I-I'm not really good-looking...", 'lia_bot')
speak("Nadine", "Oh, no haha, I just take photos for my flashcards. If you're gonna be joining Ward #6, I want to remember you.", 'd1_nadine0')
speak("Lia", "Flashcards?", 'lia_bot')
speak("Nadine", "Here. See?", 'd1_nadine0')
card('flashcards')
speak("Lia", "That's... uhm...", 'lia_bot')
speak("Nadine", "My family's going to visit any day now, so I need to be prepared.", 'd1_nadine0')
speak("Nadine", "Do you have any symptoms yet? N9 kind of sucks.", 'd1_nadine0')
speak("Lia", "No real symptoms... I don't think.", 'lia_bot')
speak("Nadine", "Well, you should come help me practice my cards sometime. I'm either here or in the common room.", 'd1_nadine0')
speak("Nadine", "And stick close to Tess. She's great!", 'd1_nadine0')
speak("Tess", "Thanks Nadine. Take care.")

setSwitch('day1_05_intro_nadine', true)
