import React, { useId } from 'react';

interface DialogBodyWidgetProps {
  id: string;
  children?: React.ReactNode;
}

export const DialogBodyWidget: React.FC<DialogBodyWidgetProps> = ({
  id,
  children,
}) => {
  const descriptionId = useId();

  return (
    <section
      id={id}
      className="flex-1 min-h-0 flex flex-col overflow-auto p-4"
      role="document"
      aria-describedby={descriptionId}
    >
      <div className="flex-1 min-h-0" id={descriptionId}>
        {children}
      </div>
    </section>
  );
};
