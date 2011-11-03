@echo %1
@echo %2
@echo %3
@if not exist %3\%1 goto create
@rmdir /S /Q %3\%1
:create
@mkdir %3\%1
@xcopy /E %2\%1 %3\%1
