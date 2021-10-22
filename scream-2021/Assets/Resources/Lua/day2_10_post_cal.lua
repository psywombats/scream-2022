if getSwitch('day2_10_post_cal') then
    speak("Dr. Cooper", "I'm sorry.")
    return
end

speak("Dr. Cooper", "Oh, Tess. I heard about Connie.", 'd2_cal4')
speak("Tess", "It's unfortunate.")
speak("Dr. Cooper", "Yeah, I...", 'd2_cal4')
speak("Dr. Cooper", "I'm so sorry.", 'd2_cal4')
speak("Tess", "Sorry for what?")
speak("Dr. Cooper", "It's an expression of empathy.", 'd2_cal4')
speak("Tess", "It's Joey that's torn up. Not me.")
speak("Tess", "Do you remember searching for Connie yesterday?")
speak("Dr. Cooper", "I wish I could give you the answer you wanted, Tess.", 'd2_cal4')
speak("Tess", "I just want a yes or no.")
speak("Dr. Cooper", "I'm sorry.", 'd2_cal4')

setSwitch('day2_10_post_cal', true)
