﻿namespace MovieHub.Application.Common.Exceptions;

public class NotFoundException(string message) : Exception(message);