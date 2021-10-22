if getSwitch('day3_02_nadine') then
    speak("Nadine", "I'll see you around, Tess! I'm sure we'll be good friends in the future.", 'd3_nadine0')
    return
end

speak("Nadine", "Hullo, ummm uhhh...", 'd3_nadine0')
speak("Nadine", "Lia! Hi Lia.", 'd3_nadine0')
speak("Lia", "Hi Nadine. This is - ", 'lia_bot')
speak("Nadine", "Wait! Hang on, let me guess...", 'd3_nadine0')
speak("Nadine", "...", 'd3_nadine0')
speak("Nadine", "Nothing, sorry. I need to hit the cards, it looks like.", 'd3_nadine0')
speak("Lia", "This is Tess. She's new here.", 'lia_bot')
speak("Nadine", "Oh, alright. Bit of a quiet one, are you?", 'd3_nadine0')
speak("Lia", "N9 got her speech pathways. Look, see what she's typing on the screen?", 'lia_bot')
speak("Tess", "Please remember me, Nadine.")
speak("Nadine", "Haha, I'll do my best, Tess. But sometimes that's just how it is.", 'd3_nadine0')
speak("Nadine", "I know! Hold still and I'll get my camera.", 'd3_nadine0')
speak("Tess", "That isn't necessary.")
speak("Nadine", "You've got such a cute face though! I wouldn't want to forget it.", 'd3_nadine0')
speak("Tess", "It was nice to meet you. Goodbye.")
faceTo('hero', 'lia_bot')
wait(.5)
speak("Lia", "U-uhm, isn't that a bit rude...?", 'lia_bot')
speak("Nadine", "Ahh, it's okay. Joining Ward #6 is tough.", 'd3_nadine0')
speak("Nadine", "I'll see you around, Tess! I'm sure we'll be good friends in the future.", 'd3_nadine0')

setSwitch('day3_02_nadine', true)
play('day3_intro')
