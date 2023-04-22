if not getSwitch('pt1_08') then
	speak('ARIEL', "The midday sky. From the 37th floor, you can see all of Aquila University, and most of the city too.")
elseif not getSwitch('pt1_10') then
	speak('ARIEL', "It's late afternoon. Looks like the sun is starting to go down.")
elseif not getSwitch('') then
	speak('ARIEL', "Night in the city. Tens of thousands of streetlights and headlamps are refracted in the clouds.")
else
	speak('ARIEL', "The witching hour. I am a lucid dreamer. I control my reality.")
end
