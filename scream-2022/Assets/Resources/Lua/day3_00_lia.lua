if getSwitch('day3_00_lia') then
    speak("Lia", "U-uhm, do you know where you're headed?", 'd3_lia0')
    speak("Lia", "I have a lot of friends here. I'm supposed to introduce you.", 'd3_lia0')
    return
end

speak("Lia", "Oh. You're finally awake.", 'd3_lia0')
speak("Tess", "Sorry. It was rough last night.")
speak("Lia", "I didn't even hear you come in. I'm Lia, by the way.", 'd3_lia0')
speak("Tess", "Yes?")
speak("Lia", "Uhm, well, welcome to Ward #6. It looks like we'll be roommates, so, it's nice to meet you.", 'd3_lia0')
speak("Lia", "Your name is Tess, right?", 'd3_lia0')
speak("Tess", "Yes.")
speak("Lia", "I'm sorry I haven't cleaned out all the stuff that's still here from my old roommate.", 'd3_lia0')
speak("Tess", "Those are my books. Don't touch them.")
speak("Tess", "I'm worried about you. You should probably see Dr. Cooper.")
speak("Lia", "You mean... Cal? I'm not sure what you're talking about.", 'd3_lia0')
speak("Lia", "Let me know when you're ready and I can give you a tour of Ward #6.", 'd3_lia0')
speak("Lia", "Allsaints is a pediatric hospital, so everyone's our age here. And all N9 positive.", 'd3_lia0')
speak("Tess", "Don't do this to me Lia.")
speak("Lia", "I-I don't mind at all. I have a lot of friends here. I'm supposed to introduce you.", 'd3_lia0')
speak("Tess", "Lia, we've met before. Can you not remember?")
speak("Lia", "How much do you know about Neural-9?", 'd3_lia0')
speak("Lia", "It usually causes memory loss, so, it's okay if you're a little confused?", 'd3_lia0')
speak("Tess", "I am not confused.")
speak("Tess", "Please follow me.")
speak("Lia", "U-uhm, alright. Do you know where you're headed?", 'd3_lia0')

setSwitch('day3_00_lia', true)
setSwitch('spawn_lia', true)