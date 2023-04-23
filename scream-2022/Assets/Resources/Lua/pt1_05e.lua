enterNVL()
enter('SUMI', 'c')

if not getSwitch('pt1_05e') then
	speak('SUMI', "So, Ariel... Braulio never mentioned what it was exactly that you do at Lucir. Professional tour guide?")
	speak('ARIEL', "I help Braulio with the electrical systems. But, I guess more importantly, I dream.")
	expr('SUMI', 'surprise')
	speak('SUMI', "Ahaha. A professional dreamer. That's the career of the future.")
	speak('ARIEL', "I was able to lucid dream even when I was a child. It turns out that in this startup, that's a useful skill to have.")
	speak('ARIEL', "I can dream whatever I like. If Braulio wants that island vacation, then I dream of an island. Then we capture it on Recurse, and anyone can replay it. It's called 'mirroring'.")
	expr('SUMI', nil)
	speak('SUMI', "So that makes you a professional artist.")
	speak('ARIEL', "Not really. I've been told my dreams are always cold. It... might be a personality defect of mine.")
	speak('ARIEL', "Chris, er, Dr. Kowalski has the truly beautiful dreams. They're random, but, they're warm. I'm told my dreams are too precise and clinical.")
	speak('SUMI', "Those are some strange, strange hats you wear, Ariel. My time is limited though -- where to?")
else
	speak('SUMI', "Where to?")
end

if not getSwitch('pt1_05c') then
	speak('ARIEL', "I don't think our prototype Recurse is in use. Would you like to do the demo now?")
elseif not getSwitch('pt1_05a') then
	speak('ARIEL', "You should probably meet our head of research, Dr. Kowalski. He's probably in his office.")
elseif not getSwitch('pt1_05b') then
	speak('ARIEL', "The main R&D lab is on this floor. Come this way.")
elseif not getSwitch('pt1_05d') then
	speak('ARIEL', "That's about all for the tour. Braulio is probably ready now.")
end

exitNVL()
setSwitch('pt1_05e', true)
