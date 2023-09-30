from typing import Optional


class BandUnit:
    def __init__(
            self,
            Value:Optional[float]=None,
            Unit:Optional[str]=None,
                 ):
        self.Value=Value
        self.Unit=Unit