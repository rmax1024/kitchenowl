import 'package:flutter/cupertino.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:universal_platform/universal_platform.dart';

class KitchenOwlSwitch extends StatelessWidget {
  final bool value;
  final Function(bool)? onChanged;

  const KitchenOwlSwitch({super.key, required this.value, this.onChanged});

  @override
  Widget build(BuildContext context) {
    if (!kIsWeb && UniversalPlatform.isAndroid) {
      return Switch(
        value: value,
        onChanged: onChanged,
      );
    }

    return Transform.scale(
      scale: 0.9,
      child: CupertinoSwitch(
        value: value,
        activeColor: Theme.of(context).colorScheme.secondary,
        onChanged: onChanged,
      ),
    );
  }
}
