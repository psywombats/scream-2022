teleport('gazer', 'pt1a', 'NORTH')

wait(2)
rotateTo('sumi_2')
wait(.7)

enterNVL()
speak('ARIEL', "Sumi?")
enter('SUMI', 'c', 'surprised')
speak('SUMI', "Oh, you startled me.")
expr('SUMI', 'nil')
speak('SUMI', "I was just admiring the lab again.")
speak('ARIEL', "Noemi and Dr. Kowalski are setting up Recurser right now. Are you ready?")
speak('SUMI', "I can't wait.")
exitNVL()

wait(0.5)
rotateTo('chair')
wait(.5)

enterNVL()
enter('BRAULIO', 'a')
enter('CHRIS', 'd')
enter('NOEMI', 'e')
speak('CHRIS', "If you don't mind, Ms. Chey, mirroring isn't quite ready for commercial use. So far only Ariel and Noemi have pulled it off.")
speak('ARIEL', "Maybe Sumi could describe a story to me, secretly from Noemi. I'll enter a lucid dream and record it with Recurser.")
speak('ARIEL', "Then we'll enable mirroring playback, and Noemi will view it and describe it, and see how accurately it matches Sumi's request.")
enter('SUMI', 'b')
speak('SUMI', "Like a challenge. I like the sound of that.")
expr('NOEMI', 'happy')
speak('NOEMI', "Ariel's dreams are always so detailed... This should be easy.")
expr('NOEMI', nil)
speak('ARIEL', "Then, Sumi, what should I dream? You can whisper to me. Noemi's half-asleep so she can't really hear anyway.")
exit('NOEMI')
exit('CHRIS')
exit('BRAULIO')
speak('SUMI', "Hmm... How about a dream atop a mountain? There are two goats on this mountaintop.")
expr('SUMI', 'intense')
speak('SUMI', "But one pushes the other, and it falls to its death. You can dream that, can't you?")
expr('SUMI', nil)
speak('ARIEL', "...I can try.")
exit('SUMI')
enter('CHRIS', 'd')
speak('CHRIS', "Here's the Bluepill.")
enter('BRAULIO', 'b')
speak('BRAULIO', "I'll get the electrodes hooked up.")
enter('NOEMI', 'd', 'happy')
speak('NOEMI', "Good luck Ariel. I know we can do it.")
exitNVL()

bootGazer(true)
fade('black')
wait(3)
setSwitch('pt1_08', true)
play('pt1_08b')