﻿namespace Messages;

public record RaceCreatedMessage(Guid Id,string Name, string TypeOfCar, string timestamp);